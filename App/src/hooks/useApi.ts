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
  const code = user?.code;

  return {
    fetchQuests: () => fetchQuests(code),
    completeQuest: (id: string) => completeQuest(id, code),
    addQuest: (quest: Quest) => addQuest(quest, code),
    fetchRewards: () => fetchRewards(code),
    redeemReward: (id: string) => redeemReward(id, code),
    addReward: (reward: Reward) => addReward(reward, code),
    deleteReward: (id: string) => deleteReward(id, code),
    fetchAccount: () => fetchAccount(code),
  };
}
