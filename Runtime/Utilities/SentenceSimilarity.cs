using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace HuggingFace.API {
    public class SentenceSimilarity {
        public float maxScore;
        public int maxScoreIndex;

        /// <summary>
        /// Find the best similiarity score
        /// </summary>
        /// <param name="similarityScores">An array of similarity scores between each context phrase and the sentence.</param>
        /// <returns>The index of the maximum similarity score.</returns>
        public float FindBestSimilarityScoreValue(float[] similarityScores)
        {
            maxScore = similarityScores.Max();
            return maxScore;
        }

        /// <summary>
        /// Finds the index of the maximum similarity score in the given array of similarity scores.
        /// </summary>
        /// <param name="similarityScores">An array of similarity scores representing the similarity between each context phrase and the sentence.</param>
        /// <returns>The index of the maximum similarity score in the array.</returns>
        public int FindBestSimilarityScoreIndex(float[] similarityScores)
        {
            maxScore = FindBestSimilarityScoreValue(similarityScores);
            maxScoreIndex = similarityScores.ToList().IndexOf(maxScore);
            
            return maxScoreIndex;
        }
    }
}