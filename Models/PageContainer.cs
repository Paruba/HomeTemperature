using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boiler.Mobile.Models;

public class PageContainer<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
}