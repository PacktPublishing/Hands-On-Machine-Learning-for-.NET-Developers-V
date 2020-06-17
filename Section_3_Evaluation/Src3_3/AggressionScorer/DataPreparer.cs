using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AggressionScorer
{
    public class DataPreparer
    {
        public static void CreatePreparedDataFile(string outputFile, bool onlySaveSmallSubset = false)
        {
            var annotations = File.ReadAllLines("Data\\aggression_annotations.tsv").Skip(1);

            var aggressiveScoreMap = new Dictionary<int, List<int>>();

            //Collect all aggression ratings for each comment (revId)
            foreach (var annotation in annotations)
            {
                var parts = annotation.Split('\t');
             
                var revId = int.Parse(parts[0]);
                var aggressiveScore = (int)double.Parse(parts[3], CultureInfo.InvariantCulture);

                if (aggressiveScoreMap.ContainsKey(revId))
                {

                    aggressiveScoreMap[revId].Add(aggressiveScore);

                }
                else
                {
                    aggressiveScoreMap[revId] = new List<int>() { aggressiveScore };
                }
            }

            // Pair all comments with aggression score
            var allComments = File.ReadAllLines("Data\\aggression_annotated_comments.tsv").Skip(1);

            var formattedOutput = allComments.Select(c =>
            {
                var inputLineParts = c.Split('\t');

                var commentId = int.Parse(inputLineParts[0]);

                var aggressionScores = aggressiveScoreMap[commentId];

                var aggression = aggressionScores.Average() < -0.9 ? 1 : 0;

                var comment = inputLineParts[1].Replace("NEWLINE_TOKEN", "");
                return $"{aggression}\t{comment}";
            });

            // Take the small or the big subset of the data
            var finalOutput = onlySaveSmallSubset ?
                formattedOutput.Take(3000).ToList() :
                formattedOutput.Skip(3000).ToList();

            //var finalOutput = formattedOutput.ToList();

            finalOutput.Insert(0, "IsAggressive\tComment");
 
            //Write the new file to use as ML.Net input
            File.WriteAllLines(outputFile, finalOutput);
        }
    }
}