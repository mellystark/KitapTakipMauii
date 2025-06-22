﻿namespace KitapTakipMauii.ViewModels;

public class ListItemViewModel
{
    public string Id { get; set; }
    public string Title { get; set; }      
    public string SubTitle { get; set; }   
    public bool IsUser { get; set; }
    public DateTime? CreatedDate { get; set; } // For books
    public DateTime? UpdatedDate { get; set; }
}
