//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ForumProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Messages
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdTopic { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Text { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public System.DateTime Datetime { get; set; }

        public virtual Topics Topics { get; set; }
        public virtual Users Users { get; set; }
    }
}
