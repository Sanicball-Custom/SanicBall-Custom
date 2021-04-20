public class Achievement
{
    public bool achieved;
    public string title, description;
    
    public Achievement(string _title, string _description)
    {
        achieved = false;
        title = _title;
        description = _description;
    }
}
