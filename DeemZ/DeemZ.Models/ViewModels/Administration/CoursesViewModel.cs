namespace DeemZ.Models.ViewModels.Administration
{
    using System;

    public class CoursesViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Credits { get; set; }
        public decimal Price { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as CoursesViewModel;

            if (item == null)
            {
                return false;
            }

            return this.Id.Equals(item.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
