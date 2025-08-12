namespace TrainingTracker.Application.DTOs.GraphQL.Exercise
{
    public class ExercisesConnection
    {
        public List<ExerciseEdge> Edges { get; set; } = new();
        public PageInfo PageInfo { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class ExerciseEdge
    {
        public string Cursor { get; set; } = string.Empty;
        public ExerciseGraphQLDto Node { get; set; }
    }

    public class PageInfo
    {
        public string StartCursor { get; set; } = string.Empty;
        public string EndCursor { get; set; } = string.Empty;
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

}
