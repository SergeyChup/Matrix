using System.Text;
using System.Web.Mvc;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICreateRandomArray _randomArray;
        private readonly ICreateFile _file;
        public HomeController(ICreateRandomArray randomArray, ICreateFile file)
        {
            this._randomArray = randomArray;
            this._file = file;
        }

        [HttpGet]
        public ActionResult Index(){ return View("Index"); }

        [HttpGet]
        public ActionResult GenerateRandomMatrix(int col = 3, int row = 3)
        {
            var matrix = _randomArray.CreateRandomArray(col, row, 10, 90);

            int[][] numbers = new int[col][];

            for (int i = 0; i < col; i++)
            {
                numbers[i] = new int[row];
                for (int j = 0; j < row; j++)
                {
                    numbers[i][j] = matrix[i, j];
                }
            }

            return Json(numbers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExportToCSV(int[][] matrix)
        {
            string csvName = "matrix.csv";
            Session[csvName] = _file.CreateFile(matrix).ToString();
            return Json(csvName);
        }
        
        public ActionResult DownloadFile(string fileName)
        {
            var fn = Session[fileName];
            if (fn == null) { return new EmptyResult(); }
            Session[fileName] = null;
            return File(Encoding.Default.GetBytes(fn.ToString()), "application/csv", fileName);
        }
    }
}