using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Collections.Generic;
using MyMvc.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace MyMvc.Controllers
{
    public class JogosController : Controller
    {

        public string uriBase = "http://brapi2.somee.com/BrApi2/Jogos/";

        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                string uriComplementar = "GetAll";
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await httpClient.GetAsync(uriBase + uriComplementar);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<JogoViewModel> listaJogos = await Task.Run(() =>
                        JsonConvert.DeserializeObject<List<JogoViewModel>>(serialized));

                    return View(listaJogos);
                }
                else
                    throw new System.Exception(serialized);
            }
            catch(System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(JogoViewModel a)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(a));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase, content);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] = string.Format("Jogo {0}, Id {1} salvo comsucesso!!!", a.Nome, serialized);
                    return RedirectToAction("Index");
                }
                else
                    throw new System.Exception(serialized);

            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Create");
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
    
        [HttpGet]
        public async Task<ActionResult> DetailsAsync(int? id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.GetAsync(uriBase + id.ToString());
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JogoViewModel a = await Task.Run(() =>
                        JsonConvert.DeserializeObject<JogoViewModel>(serialized));
                    return View(a);
                }
                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditAsync(int? id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.GetAsync(uriBase + id.ToString());

                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JogoViewModel a = await Task.Run(() =>
                        JsonConvert.DeserializeObject<JogoViewModel>(serialized));
                    return View(a);
                }
                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public async Task<ActionResult> EditAsync(JogoViewModel a)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent(JsonConvert.SerializeObject(a));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await httpClient.PutAsync(uriBase, content);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] = string.Format("Jogo {0}, Id {1} salva com sucesso!!!", a.Nome, serialized);
                    return RedirectToAction("Index");
                }
                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    
        [HttpGet]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await httpClient.DeleteAsync(uriBase + id.ToString());
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] = string.Format("Jogo Id {0} removida com sucesso!!!", id);
                    return RedirectToAction("Index");
                }
                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        
       [HttpGet]
        public async Task<ActionResult> favoritarAsync(int jogoId)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                int usuarioId = 1;

                string uriComplementar = "favoritar/" + jogoId + "/" + usuarioId;

                HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, null);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] = "Jogo favoritado com sucesso!";
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new System.Exception("Jogo ou usuário não encontrado.");
                }
                else
                {
                    throw new System.Exception(serialized);
                }
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

    
    }
}