# Sipariş ve Stok Yönetimi Projesi

Sistemin temel amacı, mağazalardaki ürünlerin stok seviyesini takip etmek ve ürünler kritik stok seviyesinin altına düştüğünde uygun tedarikçiden (FakeStoreAPI) otomatik olarak sipariş oluşturmaktır.

## Mimari

Proje, Clean Architecture prensipleri üzerine kurulmuştur ve .NET 8 kullanılarak geliştirilmiştir. Bu mimari, projenin katmanlı, genişletilebilir, test edilebilir ve bakımı kolay olmasını hedefler.

- **Domain:** Projenin iş kurallarını ve varlıklarını içerir.
- **Application:** Uygulamanın kullanım senaryolarını (use cases) ve iş akışlarını yönetir.
- **Infrastructure:** Veritabanı, API istekleri ile ilgili implementasyonları barındırır. Bu projede In-Memory Repository ve `HttpClient` ile Fake Store API istemcisi bulunmaktadır.
- **API:** RESTful endpoint'leri, güvenlik yapılandırmalarını ve istek/cevap modellerini içerir.

## Proje Nasıl Çalıştırılır?

1.  Projeyi klonlayın: `git clone <repo-url>`
2.  API projesi dizinine gidin: `cd SupplyChain.API`
3.  Projeyi çalıştırın: `dotnet run`
4.  Uygulama varsayılan olarak https://localhost:7281/swagger/index.html  çalışır.

## API 

### POST /api/products

Sisteme yeni bir ürün ekler. Bu ürün, FakeStoreAPI'deki bir ürünle eşleştirilecektir.

**Örnek Request :**

```json
{
  "name": "Elma",
  "initialStock": 25,
  "stockThreshold": 10
}
```

**Örnek response:**

```json
{
  "id": 1,
  "productCode": "FS-1",
  "name": "Elma",
  "stockQuantity": 25,
  "stockThreshold": 10
}
```

### GET /api/products/low-stock

Stok miktarı, belirlenen eşik değerin altında olan tüm ürünleri listeler.

**Örnek response :**

```json
[
  {
    "id": 3,
    "productCode": "FS-3",
    "name": "Avakado",
    "stockQuantity": 25,
    "stockThreshold": 40
  }
]
```

### POST /api/orders/check-and-place

Kritik stok seviyesindeki tüm ürünleri kontrol eder ve her biri için FakeStoreAPI'den en ucuz fiyata sahip olanı bularak sipariş oluşturma sürecini simüle eder.

**Örnek response:**

```json
{
  "message": "Sipariş kontrolü tamamlandı.",
  "placedOrders": [
    {
      "productName": "Avakado",
      "productCode": "FS-3",
      "quantityOrdered": 15,
      "price": 22.3
    }
  ]
}
```

## Fake Store API ile Eşleştirme

-   Sisteme /api/products ile bir ürün eklendiğinde, bu ürün için benzersiz bir productCode oluşturulur.
-   Bu productCode, FS-{id} formatındadır. Buradaki {id}, Fake Store API'deki bir ürünün id'sini temsil eder.
-   Sipariş otomasyonu  /orders/check-and-place çalıştığında, sistem kritik stoktaki bir ürünün productCode'unu kullanarak FakeStoreAPI'de tam olarak hangi ürünü arayacağını bilir ve en uygun fiyatlısını seçer.

## Bonus Görev: Roma Rakamı Dönüşümü

Bonus görev, stok adetlerinin kullanıcıya HTML ortamında Roma rakamları ile gösterilmesiydi. Bu görev, istenildiği gibi javascript ile çözülmüştür.

-   Algoritma, SupplyChain.API/wwwroot/index.html dosyasındaki convertToRoman fonksiyonunda bulunabilir.
-   Fonksiyon, sayıları ve Roma sembollerini içeren bir harita (map) kullanır. Harita, en büyük değerden en küçüğe doğru sıralanmıştır.
-   Verilen sayıyı bu haritadaki değerlerle karşılaştırarak, büyükten küçüğe doğru sembolleri birleştirir ve yanıtı oluşturur. 

## Not
Bu dokümanı https://readme.so aracılığı ile görselleştirdim
