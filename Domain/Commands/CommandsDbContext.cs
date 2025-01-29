using ProductsAPI.Domain.Entities.Class;
using ProductsAPI.Domain.Entities.Organizer;
using ProductsAPI.Domain.Entities.Question;
using ProductsAPI.Domain.Entities.QuestionOption;
using ProductsAPI.Domain.Entities.Subject;
using ProductsAPI.Domain.Entities.Theme;
using ProductsAPI.Domain.Entities.User;
using ProductsAPI.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Domain.Commands
{
    public class CommandsDbContext(DbContextOptions<CommandsDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<OrganizerEntity> Organizer { get; set; }
        public DbSet<SubjectEntity> Subjects { get; set; }
        public DbSet<ThemeEntity> Themes { get; set; }
        public DbSet<ClassEntity> Classes { get; set; }
        public DbSet<QuestionEntity> Questions { get; set; }
        public DbSet<QuestionOptionEntity> QuestionOptions { get; set; }
        public List<LogChangedEntry> LogChangedEntries { get; set; } = new List<LogChangedEntry>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserDbConfig());
            builder.ApplyConfiguration(new OrganizerDbConfig());
            builder.ApplyConfiguration(new SubjectDbConfig());
            builder.ApplyConfiguration(new ThemeDbConfig());
            builder.ApplyConfiguration(new ClassDbConfig());
            builder.ApplyConfiguration(new QuestionDbConfig());
            builder.ApplyConfiguration(new QuestionOptionDbConfig());
        }

        public async Task<(bool CanConnect, string ErrorMessage)> TryConnectionAsync()
        {
            try
            {
                var canConnect = await this.Database.CanConnectAsync();
                if (canConnect)
                    return (true, "");
                await this.Database.OpenConnectionAsync();
                this.Database.CloseConnection();
                return (false, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var now = DateTimeOffset.Now;
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Unchanged)
                    continue;

                var logChangedEntry = new LogChangedEntry()
                {
                    Id = entry.Property("Id").CurrentValue.ToString(),
                    EntityName = entry.Entity.GetType().Name.ToString(),
                    EntityState = entry.State.ToString()
                };

                foreach (var property in entry.Properties)
                {
                    if (entry.State != EntityState.Added && !property.IsModified)
                        continue;

                    if (entry.State != EntityState.Added && (property.OriginalValue == null || property.OriginalValue.Equals(property.CurrentValue)))
                        continue;

                    if (property.Metadata.Name == "LastModified")
                        continue;

                    logChangedEntry.Properties.Add(new LogChangedProperty()
                    {
                        PropertyName = property.Metadata.Name,
                        OriginalValue = entry.State == EntityState.Added ? null : property.OriginalValue,
                        CurrentValue = property.CurrentValue
                    });
                }

                if (logChangedEntry.Properties.Count == 0)
                    continue;

                if (entry.Entity is ITrackedEntity trEntity && !trEntity.IsOfflineCommand)
                    trEntity.LastModified = now;

                LogChangedEntries.Add(logChangedEntry);
            }
        }
    }
}