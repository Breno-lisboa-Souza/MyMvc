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
    public class FavoritosController : Controller
    {

        public string uriBase = "http://BrApi.somee.com/BrApi/Favoritos/";

        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                int usuarioId = 1; // Defina o ID do usuário corretamente

                string uriComplementar = "favoritos/" + usuarioId;
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await httpClient.GetAsync(uriBase + uriComplementar);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<FavoritoViewModel> favoritos = await Task.Run(() =>
                        JsonConvert.DeserializeObject<List<FavoritoViewModel>>(serialized));

                    return View(favoritos);
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




        [HttpGet]
        public async Task<ActionResult> favoritarAsync(int jogoId)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                int usuarioId = 0;

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

        [HttpGet]
        public async Task<ActionResult> GetFavoritosAsync(int usuarioId)
        {
            try
            {
                string uriComplementar = "favoritos/" + usuarioId;
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await httpClient.GetAsync(uriBase + uriComplementar);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<JogoViewModel> jogosFavoritos = await Task.Run(() =>
                        JsonConvert.DeserializeObject<List<JogoViewModel>>(serialized));

                    return View(jogosFavoritos);
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