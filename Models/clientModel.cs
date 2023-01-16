using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace client.Model
{
    [Table("Clients")]
    [Serializable]
    public class Client
    {
        [Column("id")]
        public int clientId { get; set; }
        //--------------------------------------------------------------------
        [Column("uuId")]
        public string uuId { get; set; }
        //--------------------------------------------------------------------
        [Column("name")]
        public string name { get; set; }
        //--------------------------------------------------------------------
        [Column("email")]
        public string email { get; set; }
        //--------------------------------------------------------------------
        [Column("password")]
        public string password { get; set; }
        //--------------------------------------------------------------------
        [Column("cpf")]
        public string? cpf { get; set; }
        //--------------------------------------------------------------------
        [Column("cnpj")]
        public string? cnpj { get; set;}
        //--------------------------------------------------------------------
        [Column("phone")]
        public string phone { get; set; }
        //--------------------------------------------------------------------
        [Column("cep")]
        public string cep { get; set; }
        //--------------------------------------------------------------------
        [Column("state")] 
        public string state { get; set; }
        //--------------------------------------------------------------------
        [Column("city")]
        public string city { get; set; }
        //--------------------------------------------------------------------
        [Column("district")]
        public string district { get; set; }
        //--------------------------------------------------------------------
        [Column("street")]
        public string street { get; set; }
        //--------------------------------------------------------------------
        [Column("number")]
        public string number { get; set; }
        //--------------------------------------------------------------------
        [Column("complement")]
        public string complement { get; set; }    
    }
}
