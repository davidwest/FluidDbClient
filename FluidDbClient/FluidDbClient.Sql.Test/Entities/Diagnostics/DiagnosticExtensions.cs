using System.Linq;
using System.Text;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace FluidDbClient.Sql.Test.Entities
{
    public static class DiagnosticExtensions
    {
        public static bool HasTheSameValue(this Composite c1, Composite c2)
        {
            return
                c1.Id == c2.Id &&
                c1.Name == c2.Name &&
                c1.Components.Count == c2.Components.Count &&
                c1.Components
                    .Zip(c2.Components, (test1, test2) => new { test1, test2 })
                    .All(pair => HasTheSameValue(pair.test1, pair.test2));
        }

        public static bool HasTheSameValue(this Component c1, Component c2)
        {
            return
                c1.Id == c2.Id &&
                c1.Name == c2.Name &&
                c1.Style == c2.Style &&
                HasTheSameValue(c1.MaintenanceSchedule, c2.MaintenanceSchedule) &&
                HasTheSameValue(c1.ReviewSchedule, c1.ReviewSchedule) &&
                c1.Widgets.Count == c2.Widgets.Count &&
                c1.Widgets
                    .Zip(c2.Widgets, (test1, test2) => new { test1, test2 })
                    .All(pair => HasTheSameValue(pair.test1, pair.test2));
        }

        public static bool HasTheSameValue(this Schedule s1, Schedule s2)
        {
            return s1.StartDate == s2.StartDate && s1.Frequency == s2.Frequency;
        }

        public static bool HasTheSameValue(this Widget w1, Widget w2)
        {
            return
                w1.Id == w2.Id &&
                w1.ExternalId == w2.ExternalId &&
                w1.IsArchived == w2.IsArchived &&
                w1.Environment == w2.Environment &&
                w1.Name == w2.Name &&
                w1.Cost == w2.Cost &&
                w1.CreatedTimestamp == w2.CreatedTimestamp &&
                w1.ReleaseDate == w2.ReleaseDate &&
                w1.SerialCode.SequenceEqual(w2.SerialCode) &&
                w1.Weight == w2.Weight &&
                w1.Rating == w2.Rating;
        }

        public static string ToDiagnosticString(this Composite composite)
        {
            var builder = new StringBuilder();

            builder
                .AppendLine()
                .AppendLine($"{composite.Id, -6} {composite.Name}")
                .AppendLine("COMPONENTS:");

            foreach (var comp in composite.Components)
            {
                builder.AppendComponent(comp, "     ");
            }

            return builder.ToString();
        }

        public static string ToDiagnosticString(this Widget widget)
        {
            var builder = new StringBuilder();

            builder.AppendWidget(widget);

            return builder.ToString();
        }

        private static void AppendComponent(this StringBuilder builder, Component component, string spacer = "")
        {
            builder
                .AppendLine()
                .AppendLine($"{spacer}Id:           {component.Id}")
                .AppendLine($"{spacer}Name:         {component.Name}")
                .AppendLine($"{spacer}Style:        {component.Style}")
                .AppendLine($"{spacer}Maint. Sched: {component.MaintenanceSchedule.StartDate:d} {component.MaintenanceSchedule.Frequency}")
                .AppendLine($"{spacer}Review Sched: {component.ReviewSchedule.StartDate:d} {component.ReviewSchedule.Frequency}")
                .AppendLine($"{spacer}WIDGETS:");

            foreach (var widget in component.Widgets)
            {
                builder.AppendWidget(widget, "          ");
            }
        }
        
        private static void AppendWidget(this StringBuilder builder, Widget widget, string spacer = "")
        {
            builder
                .AppendLine()
                .AppendLine($"{spacer}Id:           {widget.Id}")
                .AppendLine($"{spacer}External Id:  {widget.ExternalId}")
                .AppendLine($"{spacer}Environment:  {widget.Environment}")
                .AppendLine($"{spacer}Archived:     {widget.IsArchived}")
                .AppendLine($"{spacer}Name:         {widget.Name}")
                .AppendLine($"{spacer}Created:      {widget.CreatedTimestamp}")
                .AppendLine($"{spacer}Release Date: {widget.ReleaseDate:d}")
                .AppendLine($"{spacer}Serial Code:  {string.Join(" ",widget.SerialCode.Select(b => $"{b:X2}"))}")
                .AppendLine($"{spacer}Cost:         {widget.Cost:C2}")
                .AppendLine($"{spacer}Weight:       {widget.Weight}")
                .AppendLine($"{spacer}Rating:       {widget.Rating}");
        }
    }
}
