using HealthVet.Models;
using HealthVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;


namespace HealthVet.Controllers
{
    public class PetsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Breeds()
        {
            PetsDAO breeds = new PetsDAO();
            List<BreedsModel> listBreeds = breeds.GetAllBreeds();
            ViewBag.BreedsList = new SelectList(listBreeds, "id", "fullbreed");
            return View(listBreeds);
        }

        [Authorize(Roles = "cliente")]
        public IActionResult MyPets(int userId)
        {
            PetsDAO pets = new PetsDAO();
            int currentId = (int)HttpContext.Session.GetInt32("id");
            List<PetsModel> foundPets = pets.GetUserPets(currentId);
            return View(foundPets);
        }

        [Authorize(Roles = "cliente")]
        public IActionResult AddPet()
        {
            PetsDAO breeds = new PetsDAO();
            List<BreedsModel> listBreeds = breeds.GetAllBreeds();
            ViewBag.BreedsList = new SelectList(listBreeds, "id", "fullbreed");
            return View();
        }

        
        [Authorize(Roles = "cliente")]
        public IActionResult ProcessAddPet(PetsModel pet)
        {
            int currentId = (int)HttpContext.Session.GetInt32("id");
            pet.user_id = currentId;
            PetsDAO pets = new PetsDAO();
            pets.InsertPet(pet);
            return View("MyPets", pets.GetUserPets(currentId));
        }

        [Authorize(Roles = "cliente")]
        public IActionResult NewAppointment()
        {
            int currentId = (int)HttpContext.Session.GetInt32("id");
            PetsDAO pets = new PetsDAO();
            List<PetsModel> listPets = pets.GetUserPets(currentId);
            ViewBag.MyPetsList = new SelectList(listPets, "id", "name");
            List<CategoriesModel> listCategories = pets.GetAllCategories();
            ViewBag.Categories = new SelectList(listCategories, "id", "name");
            return View();
        }

        [Authorize(Roles = "cliente")]
        public IActionResult ProcessNewAppointment(AppointmentsModel appointment)
        {
            PetsDAO appointments = new PetsDAO();
            appointment.user_id = (int)HttpContext.Session.GetInt32("id");
            if (appointments.AddAppointment(appointment) == -1)
            {
                TempData["Booked"] = "Error. La fecha/hora elegida no se encuentra disponible, favor elegir nueva fecha/hora";
                List<PetsModel> listPets = appointments.GetUserPets(appointment.user_id);
                ViewBag.MyPetsList = new SelectList(listPets, "id", "name");
                List<CategoriesModel> listCategories = appointments.GetAllCategories();
                ViewBag.Categories = new SelectList(listCategories, "id", "name");
                return View("NewAppointment");
            }
            else
            {
                TempData["Appointment"] = appointment.datetime;
                return Redirect("AppointmentConfirmation");
            }
        }


        [Authorize(Roles = "cliente")]
        public IActionResult AppointmentConfirmation()
        {
            PetsDAO pets = new PetsDAO();
            int currentId = (int)HttpContext.Session.GetInt32("id");
            AppointmentsViewModel foundAppointment = pets.GetUserAppointmentByDate(currentId, (DateTime)TempData["Appointment"]);
            TempData["pet"] = foundAppointment.pet_name;
            TempData["date"] = foundAppointment.datetime;
            TempData["category"] = foundAppointment.category_name;
            return View(foundAppointment);
        }

        [Authorize(Roles = "cliente")]
        public IActionResult confirmationEmail()
        {
            PetsDAO appointments = new PetsDAO();
            int currentId = (int)HttpContext.Session.GetInt32("id");
            string currentEmail = (string)HttpContext.Session.GetString("email");
            //crear el mensaje que va a contener los datos. 
            MimeMessage message = new MimeMessage();
            //agregar direccion que envia
            message.From.Add(new MailboxAddress("Veterinaria HealthVet", "ERQDummy@gmail.com"));
            //direccion de envio
            message.To.Add(MailboxAddress.Parse(currentEmail));
            //agregar subject
            message.Subject = "Cita Agendada con Éxito";
            message.Body = new TextPart("plain")
            {
                Text = @"Gracias por confiar en nosotros!
                    Hemos programado satisfactoriamente la cita de "+ TempData["pet"] + " para " + TempData["category"] + @". 
                    Fecha y Hora: " + TempData["date"] +@".
                    Favor presentarse 15 minutos antes de la cita.
                    

                    Gracias!"
            };

            SmtpClient client = new SmtpClient();

            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("ERQDummy@gmail.com", "W3FyRzywCMM4Rse");
                client.Send(message);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }

            return View("MyAppointments", appointments.GetUserAppointments(currentId));
        }


        public IActionResult MyAppointments(int userid)
        {
            PetsDAO pets = new PetsDAO();
            int currentId = (int)HttpContext.Session.GetInt32("id");
            List<AppointmentsViewModel> foundAppointments = pets.GetUserAppointments(currentId);
            return View(foundAppointments);
        }

        [Authorize(Roles = "cliente")]
        public IActionResult ChangeAppointment(int appointmentId)
        {
            PetsDAO pets = new PetsDAO();
            AppointmentsModel foundAppointment = pets.GetAppointmentById(appointmentId);
            return View("ChangeAppointmentForm",foundAppointment);
        }

        [Authorize(Roles = "cliente")]
        public IActionResult ProcessUpdateAppointment(AppointmentsModel appointment)
        {
            PetsDAO appointments = new PetsDAO();
            int currentId = (int)HttpContext.Session.GetInt32("id");
            if (appointments.UpdateAppointment(appointment) == -1) {
            TempData["Booked"] = "Error. La fecha/hora elegida no se encuentra disponible, favor elegir nueva fecha/hora";
                return View("ChangeAppointmentForm", appointments.GetAppointmentById(appointment.id));
            }
            else { 
            return View("MyAppointments", appointments.GetUserAppointments(currentId));
        }
        }
      

    }
}
