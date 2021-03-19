using Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    public class PessoaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            IList<Pessoa> pessoas = null;
            using (var ctx = new Models.AppContext())
            {
                pessoas = ctx.Pessoas.ToList().Select(p => new Pessoa()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = p.Email,
                    Username = p.Username,
                    Password = p.Password
                }).ToList();
            }
            if (pessoas.Count == 0)
            {
                return NotFound();
            }
            return Ok(pessoas);
        }
        [HttpGet]
        public IHttpActionResult GetById(int? id)
        {
            if (id == null)
            {
                return BadRequest("O Id do contato é inválido");
            }
            Pessoa pessoa = null;
            using (var ctx = new Models.AppContext())
            {
                pessoa = ctx.Pessoas.Where(p => p.Id == id).FirstOrDefault<Pessoa>();
                //.Select(p => new Pessoa()
                //{
                //    Id = p.Id,
                //    Name = p.Name,
                //    Email = p.Email,
                //    Username = p.Username,
                //    Password = p.Password
                //})
            }
            if (pessoa == null)
            {
                return NotFound();
            }
            return Ok(pessoa);
        }
        [HttpPost]
        public IHttpActionResult PostNovoContato(PessoaDTO pessoa)
        {
            if (!ModelState.IsValid || pessoa == null)
            {
                return BadRequest("Dados do contato inválidos.");
            }
            using (var ctx = new Models.AppContext())
            {
                ctx.Pessoas.Add(new Pessoa()
                {
                    Id = pessoa.Id,
                    Name = pessoa.Name,
                    Email = pessoa.Email,
                    Username = pessoa.Username,
                    Password = pessoa.Password
                });
                ctx.SaveChanges();
            }
            return Created("",pessoa);
        }
        [HttpPut]
        public IHttpActionResult Put(Pessoa pessoa)
        {
            if (!ModelState.IsValid || pessoa == null)
                return BadRequest("Dados do contato inválidos");
            using (var ctx = new Models.AppContext())
            {
                var contatoSelecionado = ctx.Pessoas.Where(p => p.Id == pessoa.Id).FirstOrDefault<Pessoa>();
                if (contatoSelecionado != null)
                {
                    contatoSelecionado.Name = pessoa.Name;
                    contatoSelecionado.Email = pessoa.Email;
                    contatoSelecionado.Username = pessoa.Username;
                    contatoSelecionado.Password = pessoa.Password;
                    ctx.Entry(contatoSelecionado).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok($"{pessoa.Name} atualizado com sucesso");
        }
        [HttpDelete]
        public IHttpActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("Dados inválidos");
            }
            using (var ctx = new Models.AppContext())
            {
                var contatoSelecionado = ctx.Pessoas.Where(p => p.Id == id).FirstOrDefault<Pessoa>();
                if (contatoSelecionado != null)
                {
                    ctx.Entry(contatoSelecionado).State = EntityState.Deleted;
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok($"Pessoa com Id = {id} foi deletado com sucesso");
        }
    }
}
