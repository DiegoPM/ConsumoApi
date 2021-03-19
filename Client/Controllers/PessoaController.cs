using Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Client.Controllers
{
    public class PessoaController : Controller
    {
        // GET: Pessoa
        public ActionResult Index()
        {
            IEnumerable<PessoaViewModel> pessoas = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52025/api/");
                var responseTask = client.GetAsync("Pessoa");
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    pessoas = JsonConvert.DeserializeObject<IList<PessoaViewModel>>(readTask.Result);
                }
                else
                {
                    pessoas = Enumerable.Empty<PessoaViewModel>();
                    ModelState.AddModelError(string.Empty, "Erro no servidor. Contate o Administrador.");
                }
                return View(pessoas);
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(PessoaViewModel pessoa)
        {
            try
            {
                if (pessoa == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:52025/api/");
                    //Primeira forma
                    {
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                    }
                    //Segunda forma
                    {
                        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.Content = new StringContent(JsonConvert.SerializeObject(pessoa),Encoding.UTF8,"application/json");
                    var response = client.PostAsync("Pessoa", request.Content);
                    response.Wait();
                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                ModelState.AddModelError(string.Empty, "Erro no Servidor. Contacte o Administrador.");
                return View(pessoa);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PessoaViewModel pessoa = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:52025/api/Pessoa/");
                var responseTask = client.GetAsync(id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<PessoaViewModel>();
                    readTask.Wait();
                    pessoa = readTask.Result;
                }
            }
            return View(pessoa);
        }
        [HttpPost]
        public ActionResult Edit(PessoaViewModel pessoa)
        {
            if (pessoa == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:52025/api/");
                var putTask = client.PutAsJsonAsync<PessoaViewModel>("Pesssoa", pessoa);
                putTask.Wait();
                var result = putTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(pessoa);
        }
        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PessoaViewModel pessoa = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:52025/api/");
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("pessoa/" + id.ToString());
                deleteTask.Wait();
                var result = deleteTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(pessoa);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PessoaViewModel contato = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:52025/api/Pessoa");
                var responseTask = client.GetAsync("?id=" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<PessoaViewModel>();
                    readTask.Wait();
                    contato = readTask.Result;
                }
            }
            return View(contato);
        }
    }
}