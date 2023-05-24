
namespace iniciosSLO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;



class Program
{
    static void Main(string[] args)
    {
        /*
		Product product = new Product
        {
            ID = 1,
            Description = "Product Description",
            ProductCode = "ABC123",
            ProductTypeID = 2,
            SellerID = 100
        };

		Response(product);
		*/

        //CreateProduct("Test1", "ABC123", 1, 1);
        CreateProduct("Test2", "DEF456", 1, 1);
        //CreateProduct("Test3", "ZXY098", 1, 1);
    }


	// Response para testeos rest
	public static void Response(Product p)
	{
		string json = JsonConvert.SerializeObject(p);

		JObject jsonObject = JObject.Parse(json);

		foreach (var property in jsonObject)
			{
				Console.WriteLine($"{property.Key}: {property.Value}");
			}

	}

	// Crear products
	public static void CreateProduct(string Descripcion, string ProductCode, int ProductTypeID, int SellerID)
	{

		//Se define el objeto a crear

        Product p = new Product
        {
            Description = Descripcion,
            ProductCode = ProductCode,
            ProductTypeID = ProductTypeID,
            SellerID = SellerID
        };
		
		//Implementamos metod Save

		bool save = Save(p);

		if (save == false)
		{
			Console.WriteLine("Product_code duplicado");
		}


    }


	// Guardar producto
	public static bool Save(Product p)
	{

		bool exists = Exists(p.ProductCode);

		if (exists == false) { 

			// Conexion y declaracion

			MySqlConnection connection = ConexionDB();
			string query = "INSERT INTO products (descripcion, product_code, product_type_id, seller_id) VALUES (@descripcion, @productCode, @productTypeID, @sellerID)";
			MySqlCommand command = new MySqlCommand(query, connection);

			// Agrega los parámetros a la consulta

			command.Parameters.AddWithValue("@descripcion", p.Description);
			command.Parameters.AddWithValue("@productCode", p.ProductCode);
			command.Parameters.AddWithValue("@productTypeID", p.ProductTypeID);
			command.Parameters.AddWithValue("@sellerID", p.SellerID);

			// Ejecuta la consulta

			command.ExecuteNonQuery();

			// Cierre de conexion

			connection.Close();

			return true;

		}
		else
		{
			return false;
		}

	}

	//Verificamos que no exista el producto a crear
	public static bool Exists(string ProductCode)
	{

        // Conexion y declaracion

        MySqlConnection connection = ConexionDB();
        string query = "SELECT id FROM products WHERE product_code = @productCode";
        MySqlCommand command = new MySqlCommand(query, connection);

        // Agrega los parámetros a la consulta

        command.Parameters.AddWithValue("@productCode", ProductCode);

        // Ejecuta la consulta

        int rowsAffected = Convert.ToInt32(command.ExecuteScalar());

        Console.WriteLine("rows afectes es: " + rowsAffected); //Solo para verificar si hay fila afectada

        // Cierre de conexion

        connection.Close();

        if (rowsAffected > 0)

			return true;

		else

			return false;
	}



	//Conexion standard para la DB
    public static MySqlConnection ConexionDB()
	{
		string connectionString = "server=localhost;port=3306;database=SLO;user=root;password='';";

		MySqlConnection connection = new MySqlConnection(connectionString);

		connection.Open();

		return connection;

	}




}

//Objeto producto
public class Product
{
	[JsonProperty("id")]
	public int ID;

	[JsonProperty("description")]
	public string Description;

	[JsonProperty("product_code")]
	public string ProductCode;

	[JsonProperty("product_type_id")]
	public int ProductTypeID;

	[JsonProperty("seller_id")]
	public int SellerID;
}


//-----------------------------------

//BORRADORES

/*
 * string connectionString = "Server=127.0.0.1; Port=3306; Database=testeando; Uid=root; Password=root; "; MySqlConnection connection = new MySqlConnection(connectionString);
string username = txtUsername.Text;
string password = txtPassword.Text; //string datos = null;
try
{
connection.Open();
=
//string query = "SELECT COUNT(*) FROM usuario WHERE name = @Username AND password string consulta "Select name, password from usuario where name = " + username + MySqlCommand comando = new MySqlCommand(consulta); comando.Connection = connection;
=
@Password"; " and password
=
+ password + "'";
MySqlDataReader reader = comando.ExecuteReader();
if( reader.Read())
{
lblMessage.Text = "Usuario: " + userna rname+" ingreso con exito.";
}
else
{
lblMessage.Text = "Error al ingresar a la BD";
}
catch (Exception ex)
{
}
{
// Handle any exceptions that occur during database operations lblMessage.Text = "Ocurrio un error en: " + ex.Message;
finally
connection.Close();

 * 
 * type CreateProduct struct {
	//ID             int     `json:"id"`
	Description    string  `json:"description" binding:"required"`
	ExpirationRate float32 `json:"expiration_rate" binding:"required"`
	FreezingRate   float32 `json:"freezing_rate" binding:"required"`
	Height         float32 `json:"height" binding:"required"`
	Length         float32 `json:"length" binding:"required"`
	Netweight      float32 `json:"netweight" binding:"required"`
	ProductCode    string  `json:"product_code" binding:"required"`
	RecomFreezTemp float32 `json:"recommended_freezing_temperature" binding:"required"`
	Width          float32 `json:"width" binding:"required"`
	ProductTypeID  int     `json:"product_type_id" binding:"required"`
	SellerID       int     `json:"seller_id" binding:"required"`
}

func CreateProductRequest(createBody CreateProduct) domain.Product {
	createProduct := domain.Product{}
	createProduct.Description = createBody.Description
	createProduct.ExpirationRate = createBody.ExpirationRate
	createProduct.FreezingRate = createBody.FreezingRate
	createProduct.Height = createBody.Height
	createProduct.Length = createBody.Length
	createProduct.Netweight = createBody.Netweight
	createProduct.ProductCode = createBody.ProductCode
	createProduct.RecomFreezTemp = createBody.RecomFreezTemp
	createProduct.Width = createBody.Width
	createProduct.ProductTypeID = createBody.ProductTypeID
	createProduct.SellerID = createBody.SellerID

	return createProduct
}

// CreateProduct godoc
// @Summary Create product
// @Tags Products
// @Description Create product
// @Accept  json
// @Produce  json
// @Param product body CreateProduct true "ProductRequest object"
// @Success 201 {object} web.response{data=domain.Product}
// @Failure 422 {object} web.errorResponse
// @Failure 409 {object} web.errorResponse
// @Failure 500 {object} web.errorResponse
// @Router /api/v1/products [post]
func (p *Product) Create() gin.HandlerFunc {
	return func(c *gin.Context) {
		var createBody CreateProduct
		if err := c.ShouldBindJSON(&createBody); err != nil {
			web.Error(c, http.StatusUnprocessableEntity, "request body error")
			return
		}
		createProduct := CreateProductRequest(createBody)

		createdProduct, err := p.service.Save(c, createProduct)
		if err != nil {
			if errors.Is(err, product.ErrDuplicateProductCode) {
				web.Error(c, http.StatusConflict, "duplicate product code")
				return
			} else {
				web.Error(c, http.StatusInternalServerError, "internal server error")
				c.Error(err)
				return
			}
		}

		web.Success(c, http.StatusCreated, createdProduct)
	}
}


-----> SERVICE

// Create creates a new product.
func (s service) Save(ctx context.Context, p domain.Product) (domain.Product, error) {
	boolean := s.repo.Exists(ctx, p.ProductCode)
	if boolean {
		return domain.Product{}, ErrDuplicateProductCode
	}
	id, err := s.repo.Save(ctx, p)
	createdProduct, _ := s.repo.Get(ctx, id)

	return createdProduct, err
}


-----> REPO

func (r *repository) Exists(ctx context.Context, productCode string) bool {
	query := "SELECT product_code FROM products WHERE product_code=?;"
	row := r.db.QueryRow(query, productCode)
	err := row.Scan(&productCode)
	return err == nil
}

func (r *repository) Save(ctx context.Context, p domain.Product) (int, error) {
	query := "INSERT INTO products(description,expiration_rate,freezing_rate,height,length,net_weight,product_code,recommended_freezing_temperature,width,product_type_id,seller_id) VALUES (?,?,?,?,?,?,?,?,?,?,?)"
	stmt, err := r.db.Prepare(query)
	if err != nil {
		return 0, err
	}

	res, err := stmt.Exec(p.Description, p.ExpirationRate, p.FreezingRate, p.Height, p.Length, p.Netweight, p.ProductCode, p.RecomFreezTemp, p.Width, p.ProductTypeID, p.SellerID)
	if err != nil {
		return 0, err
	}

	id, err := res.LastInsertId()
	if err != nil {
		return 0, err
	}

	return int(id), nil
}


*/



// PRODUCT COMPLETO

/*
 * 
 *     static void Main(string[] args)
    
        Product product = new Product
        {
            ID = 1,
            Description = "Product Description",
            ExpirationRate = 0.5f,
            FreezingRate = 0.3f,
            Height = 10.0f,
            Length = 20.0f,
            Netweight = 2.5f,
            ProductCode = "ABC123",
            RecomFreezTemp = -18.0f,
            Width = 15.0f,
            ProductTypeID = 2,
            SellerID = 100
        };

        string json = JsonConvert.SerializeObject(product);
        Console.WriteLine(json);
    }
}
}

public struct Product
{
    [JsonProperty("id")]
    public int ID { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
  
    [JsonProperty("expiration_rate")]
    public float ExpirationRate { get; set; }

    [JsonProperty("freezing_rate")]
    public float FreezingRate { get; set; }

    [JsonProperty("height")]
    public float Height { get; set; }

    [JsonProperty("length")]
    public float Length { get; set; }

    [JsonProperty("netweight")]
    public float Netweight { get; set; }

    [JsonProperty("recommended_freezing_temperature")]
    public float RecomFreezTemp { get; set; }

    [JsonProperty("width")]
    public float Width { get; set; }
 
    [JsonProperty("product_code")]
    public string ProductCode { get; set; }

    [JsonProperty("product_type_id")]
    public int ProductTypeID { get; set; }

    [JsonProperty("seller_id")]
    public int SellerID { get; set; }
   }


*/