﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFH_USZ_PICC.Interfaces
{
    public interface IKnowledgeEntryElement
    {
        Object element { get; }
        string type { get; }
    }
}
