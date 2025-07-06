using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Models
{
    public class KitaplarRepository : Repository<Kitaplar>, IKitaplarRepository
    {
        private UygulamaDbContext _uygulamaDbContext;
        public KitaplarRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
        }

        public void Guncelle(Kitaplar kitaplar)
        {
            _uygulamaDbContext.Update(kitaplar);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
