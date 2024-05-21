<h1>N-Tier Architecture Visual Studio Extension</h1>

<p>Bu Visual Studio eklentisi, N-Tier Architecture ile çalışan projeler için gereken temel bileşenleri oluşturmak için context menü içerisine seçenekler sunar. Menüsüden "Create Dto", "Create Repository" veya "Create Service" seçerek öğelerin otomatik olarak oluşturulmasını sağlar.</p>

<h2>Kurulum</h2>
<ol>
    <li><strong>VSIX Dosyasını İndir</strong>: <a href="https://marketplace.visualstudio.com/items?itemName=BugreME.MEExtension_NTier">VisualStudio Marketplace</a> sayfasından en son sürümü indirin.</li>
    <li><strong>Yükleyin</strong>: İndirdiğiniz ".vsix" dosyasına çift tıklayarak Visual Studio'ya yükleyin.</li>
    <li><strong>Visual Studio'yu Yeniden Başlatın</strong>: Değişikliklerin etkili olması için Visual Studio'yu yeniden başlatın.</li>
</ol>

<h2>Kullanım</h2>

<img src="https://bugrame.com/images/me_extension_ntier.gif" alt="ME Extension NTier" />

<ol>
    <li><strong>Entity'den Item Oluşturma</strong>:
        <ul>
            <li>Solution Explorer üzerinden bir modele sağ tıklayın.</li>
            <li><code>Add</code> > <code>New Item</code> seçeneğini seçin.</li>
            <li>Açılan pencerede <code>Create Dto</code>, <code>Create Repository</code> veya <code>Create Service</code> seçeneğini seçin.</li>
            <li>Açılan form üzerinde oluşturulacak dosyanın proje ismini ve klasör ismini girip <code>Add</code> butonuna tıklayın.</li>
        </ul>
        <br />
    </li>
    <li><strong>Entity Örneği:</strong></li>
    <pre><code>public class Product : BaseEntity {
    public string Name { get; set; }
    public decimal Price { get; set; }
}</code></pre>
    <br />
    <li><strong>Entity'den Türetilen Örnekler:</strong>
        <ul>
            <li><strong>Dto Örneği:</strong></li>
            <pre><code>public class ProductDto : BaseDto {
    public string Name { get; set; }
    public decimal Price { get; set; }
}</code></pre>
            <li><strong>Repository Örneği:</strong></li>
            <pre><code>public class ProductRepository : BaseRepository&lt;Product&gt; {
    public ProductRepository() { }
}</code></pre>
            <li><strong>ServiceHelper Örneği:</strong></li>
            <pre><code>public class ProductService : BaseService&lt;ProductRepository, ProductDto, Product&gt; {
    public ProductService() { }
}</code></pre>
        </ul>
    </li>
</ol>
