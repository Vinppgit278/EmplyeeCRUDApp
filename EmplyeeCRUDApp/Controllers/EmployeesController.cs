using EmplyeeCRUDApp.Data;
using EmplyeeCRUDApp.Migrations;
using EmplyeeCRUDApp.Models;
using EmplyeeCRUDApp.Repository;
using EmplyeeCRUDApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmplyeeCRUDApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeDbContext _db;
        private readonly IFileService _fileService;
        private const int PageSize = 10;
        private IEmployeeRepository _employeeRepository;
        //Initializing the _employeeRepository through parameterless constructor
       //Test for Git
        public EmployeesController(EmployeeDbContext db, IFileService fileService) 
        { _db = db; _fileService = fileService; _employeeRepository = new EmployeeRepository(db); }

        public async Task<IActionResult> Index(string sortOrder = "", string searchString = "",int pageNumber = 1)
        {
            try
            {
                int pageSize = 5; // Change page size as you want
                ViewBag.CurrentFilter = searchString;
                ViewBag.FirstNameSort = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
                ViewBag.LastNameSort = sortOrder == "last_asc" ? "last_desc" : "last_asc";
                ViewBag.DeptSort = sortOrder == "dept_asc" ? "dept_desc" : "dept_asc";
                ViewBag.DOBSort = sortOrder == "dob_asc" ? "dob_desc" : "dob_asc";
                ViewBag.HobbySort = sortOrder == "hobby_asc" ? "hobby_desc" : "hobby_asc";

                var employees = await _employeeRepository.GetAllAsync(searchString);
                    
                // Sorting logic
                switch (sortOrder)
                {
                    case "first_desc": employees = employees.OrderByDescending(e => e.FirstName); break;
                    case "last_asc": employees = employees.OrderBy(e => e.LastName); break;
                    case "last_desc": employees = employees.OrderByDescending(e => e.LastName); break;
                    case "dept_asc": employees = employees.OrderBy(e => e.Departments.DepartmentName); break;
                    case "dept_desc": employees = employees.OrderByDescending(e => e.Departments.DepartmentName); break;
                    case "dob_asc": employees = employees.OrderBy(e => e.DOB); break;
                    case "dob_desc": employees = employees.OrderByDescending(e => e.DOB); break;
                    case "hobby_asc": employees = employees.OrderBy(e => e.Hobbies); break;
                    case "hobby_desc": employees = employees.OrderByDescending(e => e.Hobbies); break;
                    default: employees = employees.OrderBy(e => e.FirstName); break;
                }

                return View(await PaginatedList<Employee>.CreateAsync(employees, pageNumber, pageSize));
            }
            catch
            {
                return View(new List<Employee>());
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetStates(int countryId)
        {
            try
            {
                var states = await _employeeRepository.GetStatesByCountryIdAsync(countryId);
                return Json(states);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error while loading states."
                });
            }
        }

        [HttpGet]

        public async Task<JsonResult> GetCities(int stateId)
        {
            try
            {
                var cities = await _employeeRepository.GetCitiesByStateIdAsync(stateId);
                return Json(cities);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error while loading cities."
                });
            }
        }

        // GET: Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var departments = await _employeeRepository.GetDepartmentListAsync();
                var countries = await _employeeRepository.GetCountryListAsync();

                ViewBag.Departments = departments;
                ViewBag.Countries = countries;

                return View();
            }
            catch (Exception ex)
            {
                // Show an error page or send friendly message
                ViewBag.ErrorMessage = "Error while loading the create page.";
                return View("Error"); // Your custom Error.cshtml
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee emp, IFormFile PhotoFile)
        {
            // Remove complex navigation objects from validation
            ModelState.Remove("Departments");
            ModelState.Remove("Countries");
            ModelState.Remove("States");
            ModelState.Remove("Cities");

            //Employee Path
            if (PhotoFile != null && PhotoFile.Length > 0)
            {
                // Generate unique filename
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(PhotoFile.FileName);

                // Create upload folder if not exists
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                // Save file
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await PhotoFile.CopyToAsync(stream);
                }

                // Save relative path to database
                emp.PhotoPath = fileName;
            }


            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeRepository.InsertAsync(emp);
                    await _employeeRepository.SaveAsync();

                    return RedirectToAction("Index", "Employees");
                }
                catch (Exception ex)
                {
                    // Optional: Log exception here

                    ModelState.AddModelError("", "Error while creating employee.");
                }
            }
            // Reload dropdowns when ModelState fails or exception occurs
            ViewBag.Departments = await _employeeRepository.GetDepartmentListAsync();
            ViewBag.Countries = await _employeeRepository.GetCountryListAsync();

            ViewBag.States = emp.CountryId > 0
                ? await _employeeRepository.GetStatesByCountryIdAsync(emp.CountryId)
                : new List<State>();

            ViewBag.Cities = emp.StateId > 0
                ? await _employeeRepository.GetCitiesByStateIdAsync(emp.StateId)
                : new List<City>();
            return View(emp);
        }
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                ViewBag.Departments = await _employeeRepository.GetDepartmentListAsync();
                ViewBag.Countries = await _employeeRepository.GetCountryListAsync();

                var emp = await _employeeRepository.GetByIdAsync(id);
                if (emp == null)
                    return NotFound();

                // Load States
                ViewBag.States = await _employeeRepository.GetStatesByCountryIdAsync(emp.CountryId);

                // Load Cities
                ViewBag.Cities = await _employeeRepository.GetCitiesByStateIdAsync(emp.StateId);

                return View(emp);
            }
            catch (Exception ex)
            {
                // Optional: log error

                ViewBag.ErrorMessage = "Error loading employee details.";
                return View("Error");  // Your Error.cshtml page
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Employee emp, IFormFile PhotoFile)
        {
            ModelState.Remove("Departments");
            ModelState.Remove("Countries");
            ModelState.Remove("States");
            ModelState.Remove("Cities");

            //Image Path
            if (PhotoFile != null && PhotoFile.Length > 0)
            {
                // Generate unique filename
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(PhotoFile.FileName);

                // Create upload folder if not exists
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                // Save file
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await PhotoFile.CopyToAsync(stream);
                }

                // Save relative path to database
                emp.PhotoPath = fileName;
            }
            ModelState.Remove("PhotoFile");
            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeRepository.UpdateAsync(emp);
                    await _employeeRepository.SaveAsync();

                    return RedirectToAction("Index", "Employees");
                }
                catch (Exception ex)
                {
                    // Optional: log exception
                    ModelState.AddModelError("", "Error while updating employee.");
                }
            }

            // Reload dropdowns when ModelState fails or exception occurs
            ViewBag.Departments = await _employeeRepository.GetDepartmentListAsync();
            ViewBag.Countries = await _employeeRepository.GetCountryListAsync();

            ViewBag.States = emp.CountryId > 0
                ? await _employeeRepository.GetStatesByCountryIdAsync(emp.CountryId)
                : new List<State>();

            ViewBag.Cities = emp.StateId > 0
                ? await _employeeRepository.GetCitiesByStateIdAsync(emp.StateId)
                : new List<City>();

            return View(emp);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                ViewBag.Departments = await _employeeRepository.GetDepartmentListAsync();
                ViewBag.Countries = await _employeeRepository.GetCountryListAsync();

                var emp = await _employeeRepository.GetByIdAsync(id);
                if (emp == null)
                    return NotFound();

                ViewBag.States = await _employeeRepository.GetStatesByCountryIdAsync(emp.CountryId);
                ViewBag.Cities = await _employeeRepository.GetCitiesByStateIdAsync(emp.StateId);

                return View(emp);
            }
            catch (Exception ex)
            {
                // log here if needed
                ViewBag.ErrorMessage = "Error loading employee details.";
                return View("Error");
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ViewBag.Departments = await _employeeRepository.GetDepartmentListAsync();
                ViewBag.Countries = await _employeeRepository.GetCountryListAsync();

                var emp = await _employeeRepository.GetByIdAsync(id);
                if (emp == null)
                    return NotFound();

                ViewBag.States = await _employeeRepository.GetStatesByCountryIdAsync(emp.CountryId);
                ViewBag.Cities = await _employeeRepository.GetCitiesByStateIdAsync(emp.StateId);

                return View(emp);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error loading employee for delete.";
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _employeeRepository.DeleteAsync(id);
                await _employeeRepository.SaveAsync();

                return RedirectToAction("Index", "Employees");
            }
            catch (Exception ex)
            {
                // optional: log error
                ViewBag.ErrorMessage = "Error while deleting employee.";
                return View("Error");
            }
        }
    }
}
