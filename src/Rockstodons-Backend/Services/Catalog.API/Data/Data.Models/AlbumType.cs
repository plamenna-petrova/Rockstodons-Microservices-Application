﻿using Catalog.API.Data.Data.Common.Models.Abstraction;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Data.Models
{
    public class AlbumType : BaseDeletableModel<string>
    {
        public string Name { get; set; } = default!;
    }
}
