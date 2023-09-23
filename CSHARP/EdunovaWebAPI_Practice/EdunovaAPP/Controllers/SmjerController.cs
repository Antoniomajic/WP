﻿using EdunovaAPP.Data;
using EdunovaAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EdunovaAPP.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SmjerController : ControllerBase
    {
        private readonly EdunovaContext _context;

        public SmjerController(EdunovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // kontrola ukoliko upit nije dobar
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var smjerovi = _context.Smjer.ToList();
                if(smjerovi==null || smjerovi.Count == 0)
                {
                    return new EmptyResult();
                }
                return new JsonResult(_context.Smjer.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);                
            } 
        }

        [HttpPost]
        public IActionResult Post(Smjer smjer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Smjer.Add(smjer);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, smjer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }

        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, Smjer smjer)
        {            
            if (sifra <= 0 || smjer==null)
            {
                return BadRequest();
            }

            try
            {
                var smjerBaza = _context.Smjer.Find(sifra);
                if (smjerBaza == null)
                {
                    return BadRequest();
                }
                // inače se rade tzv. Mapper-i
                // mi ćemo za sada ručno
                smjerBaza.Naziv = smjer.Naziv;
                smjerBaza.Trajanje=smjer.Trajanje;
                smjerBaza.Cijena=smjer.Cijena;  
                smjerBaza.Upisnina=smjer.Upisnina;
                smjerBaza.Verificiran=smjer.Verificiran;

                _context.Smjer.Update(smjerBaza);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, smjerBaza);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex); // kada se vrati cijela instanca ex tada na klijentu imamo više podataka o grešci
                // nije dobro vraćati cijeli ex ali za dev je OK
            }

            
        }

        [HttpDelete]
        [Route("{sifra:int}")]
        [Produces("application/json")]
        public IActionResult Delete(int sifra)
        {
            if (sifra <= 0)
            {
                return BadRequest();
            }

            var smjerBaza = _context.Smjer.Find(sifra);
            if (smjerBaza == null)
            {
                return BadRequest();
            }

            try
            {
                _context.Smjer.Remove(smjerBaza);
                _context.SaveChanges();

                return new JsonResult("{\"poruka\":\"Obrisano\"}");

            }
            catch (Exception ex)
            {

                return new JsonResult("{\"poruka\":\"Ne može se obrisati\"}");

            }
        }
    }
}