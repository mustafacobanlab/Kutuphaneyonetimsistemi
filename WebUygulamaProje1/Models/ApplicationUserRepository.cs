using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Models
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private UygulamaDbContext _uygulamaDbContext;
        public ApplicationUserRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
        }

        public void Guncelle(ApplicationUser applicationUser)
        {
            _uygulamaDbContext.Update(applicationUser);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
