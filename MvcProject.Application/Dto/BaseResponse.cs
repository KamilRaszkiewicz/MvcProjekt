﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto
{
    public class BaseResponse
    {
        public int Status { get; set; } // 0 is ok

        public string? Error { get; set; }
    }
}
