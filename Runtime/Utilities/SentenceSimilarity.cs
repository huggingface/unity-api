namespace HuggingFace.API {
    public class SentenceSimilarity {
        /// <summary>
        /// Find the best similiarity score
        /// </summary>
        /// <param name="similarityScores">An array of similarity scores between each context phrase and the sentence.</param>
        /// <returns>The index of the maximum similarity score.</returns>
        public float FindBestSimilarityScoreValue(float[] similarityScores)
        {
            float maxScore = scores.Max();
            return maxScore
        }

        /// <summary>
        /// Finds the index of the maximum similarity score in the given array of similarity scores.
        /// </summary>
        /// <param name="similarityScores">An array of similarity scores representing the similarity between each context phrase and the sentence.</param>
        /// <returns>The index of the maximum similarity score in the array.</returns>
        public float FindBestSimilarityScoreIndex(float[] similarityScores)
        {
            float maxScore = FindBestSimilarityScoreValue(similarityScores)
            float maxScore = similarityScores.ToList().IndexOf(maxScore);
            
            return maxScore
        }
    }
}