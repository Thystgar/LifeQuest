import { useContext } from 'react';
import { AuthContext } from '../contexts';
import type { Quest, Reward } from '../api';
import {
  fetchQuests,
  completeQuest,
  addQuest,
  fetchRewards,
  redeemReward,
  addReward,
  deleteReward,
  fetchAccount,
} from '../api';

export function useApi() {
  const user = useContext(AuthContext);
  const accessToken = user?.accessToken;

  return {
    fetchQuests: () => fetchQuests(accessToken),
    completeQuest: (id: string) => completeQuest(id, accessToken),
    addQuest: (quest: Quest) => addQuest(quest, accessToken),
    fetchRewards: () => fetchRewards(accessToken),
    redeemReward: (id: string) => redeemReward(id, accessToken),
    addReward: (reward: Reward) => addReward(reward, accessToken),
    deleteReward: (id: string) => deleteReward(id, accessToken),
    fetchAccount: () => fetchAccount(accessToken),
  };
}
