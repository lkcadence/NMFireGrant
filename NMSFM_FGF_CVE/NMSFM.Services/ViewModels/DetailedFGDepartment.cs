using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMSFM.Data;

namespace NMSFM.ViewModels
{
    [Serializable]
    public class DetailedFGDepartment
    {
        public Guid addressId { get; set; }
        public string DepartmentName { get; set; }
    }
}
