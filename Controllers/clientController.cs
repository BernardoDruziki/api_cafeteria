using System.Text.Json;
using client.Model;
using DbPgSql;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Validator;

namespace cafeteria.Controllers
{
    public class clientController : Controller
    {
        [HttpPost("/registerClient")]
        public static async Task<string> registerClient([FromBody] Client client)
        {
            string response = "";
            clientValidator validator = new clientValidator();
            ValidationResult result = validator.Validate(client);//Faz a validação do usuário.
            string message = result.ToString();
            if (!result.IsValid)
            {
                return message;
            }
            else
            {
                using (var pgsql = new pgsql())
                {
                    try
                    {
                        List<Client> IsEmailValid = pgsql.Clients.Where(x => x.email == client.email).ToList();//Verifica se o email já existe no banco.                                                    
                        if (IsEmailValid.Count == 0)
                        {//Persiste o usuário, caso ele não esteja na base.
                            List<Client> IsDocumentValid = pgsql.Clients.Where(x => x.cpf == client.cpf).Where(x => x.cnpj == client.cnpj).ToList();//Verifica se os documentos já existem no banco.
                            if (IsDocumentValid.Count == 0)
                            {
                                client.uuId = Guid.NewGuid().ToString(client.uuId);//Gera o uuId do usuário.
                                client.password = BCrypt.Net.BCrypt.HashPassword(client.password);//Encripta a senha do usuário.
                                client.email = client.email.ToString().ToLower();//Salva o email no banco em lower case.
                                pgsql.Clients.Add(client);
                                await pgsql.SaveChangesAsync();
                                message = "OK";//Usuário salvo com sucesso.
                                response = JsonSerializer.Serialize(new { client.clientId, client.uuId, message });//Retorno para o front.
                            }
                            else
                            {
                                message = "DOCUMENT_EXISTS";//Cpf ou Cnpj já existente no banco.
                                response = JsonSerializer.Serialize(new { message });
                            }
                        }
                        else
                        {
                            message = "EMAIL_EXISTS";//Email já existente no banco.
                            response = JsonSerializer.Serialize(new { message });
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return response;
                }
            }
        }

        [HttpGet("/getClient/{Id}")]
        public static async Task<string> getClient([FromRoute] int clientId)//Método para o front receber um usuário especifico.      
        {
            string response = "";
            string message = response.ToString();
            //Product product = new Product();
            using (var pgsql = new pgsql())
            {
                List<Client> dbUsers = pgsql.Clients.Where(x => x.clientId == clientId).ToList();//Acessa o banco de procuro o usuário pelo userId.
                if (dbUsers.Count > 0)//Se o userId for maior que 0, ou seja, existente, vai retornar o usuário.
                {
                    response = JsonSerializer.Serialize(new { dbUsers });
                }
                else
                {
                    message = "USER_NOT_FOUND";//Usuário não encontrado.
                    response = JsonSerializer.Serialize(new { message });
                }
            }
            return response;
        }

        [HttpPut("/editClient/{Id}")]
        public static async Task<String> editClient([FromBody] Client client)//Passar o id e uuId para edição.
        {
            string response = "";
            string message = response.ToString();
            using (var pgsql = new pgsql())
            {
                try
                {
                    var clients = new Client();
                    pgsql.Clients.Update(client);//Edita um usuário existente no bacno.
                    await pgsql.SaveChangesAsync();
                    message = "OK";//Usuário editado com sucesso.
                    response = JsonSerializer.Serialize(new { message });
                }
                catch (Exception e)
                {
                    message = "USER_NOT_FOUND";//Usuário não encontrado.
                    response = JsonSerializer.Serialize(new { message });
                }
            }
            return response;
        }

        [HttpDelete("/deleteClient/{Id}")]
        public static async Task<string> deleteClient([FromRoute] int Id)//Passar o id para deletar.
        {
            string response = "";
            string message = response.ToString();
            using (var pgsql = new pgsql())
            {
                try
                {
                    Client user = pgsql.Clients.Find(Id);//Acha o usário pelo Id no banco.
                    pgsql.Clients.Remove(user);//Deleta o usuário do banco.
                    pgsql.SaveChanges();
                    message = "OK";//Usuário deletado com sucesso.
                    response = JsonSerializer.Serialize(new { message });
                }
                catch (Exception e)
                {
                    message = "USER_NOT_FOUND";//Usuário não encontrado.
                    response = JsonSerializer.Serialize(new { message });
                }
            }
            return response;
        }
    }
}

