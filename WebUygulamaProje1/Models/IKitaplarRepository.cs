namespace WebUygulamaProje1.Models
{
    public interface IKitaplarRepository : IRepository<Kitaplar>
    {
        void Guncelle(Kitaplar kitaplar);
        void Kaydet();
    }
}
