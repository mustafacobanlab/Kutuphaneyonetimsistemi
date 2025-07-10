namespace WebUygulamaProje1.Models
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        void Guncelle(ApplicationUser applicationUser);
        void Kaydet();
    }
}
