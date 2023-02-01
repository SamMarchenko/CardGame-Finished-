using UnityEngine;

namespace DefaultNamespace
{
    public class IncreaseStatsParametersProvider
    {
        private ConfigIncreaseStatsParameters _configIncreaseStatsParameters;

        public IncreaseStatsParametersProvider(ConfigIncreaseStatsParameters configIncreaseStatsParameters)
        {
            _configIncreaseStatsParameters = configIncreaseStatsParameters;
        }

        public IncreaseStatsParameters GetIncreaseStatsParameters(CardView card)
        {
            foreach (var parameters in _configIncreaseStatsParameters.IncreaseStatsConfig)
            {
                if (card.CardId == parameters.ID)
                {
                    return parameters;
                }
            }

            return null;
        }
        
        
    }
}