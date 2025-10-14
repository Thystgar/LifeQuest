import { useAuth } from '@/hooks//useAuth';
import { Account, Quest, Reward } from '@/api';
import { Group, NewGroup } from '@/api/models/Group';

export function useApi() {
  //const BASE_URI = 'http://10.0.2.2:8080';
  const BASE_URI = "https://www.lifequest.website:8443";

  const { auth, isUserAuthenticated } = useAuth();
  
  const getAuthHeader = () =>
    isUserAuthenticated ? { 'Authorization': `Bearer ${auth}` } : undefined;

  const getUrl = (path: string) => `${BASE_URI}${path}`;

  const sendRequestAsync = async (path: string, method: string, body: any = {}) => {
    const headers = {
      ...getAuthHeader(),
      'Content-Type': 'application/json',
    };
    const url = getUrl(path);

    console.log(`Sending ${method} request to ${url} with body:`, body);

    try {
      const response = await fetch(url, {
        method,
        headers,
        ...(body && ['POST', 'PUT'].includes(method) && { body: JSON.stringify(body) })
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status} on ${method}: ${url}`);
      }
      const result = await response.json();
      console.log(`Response from ${method} ${url}:`, result);
      return result;
    }
    catch (error) {
      console.error(`Error sending request ${method}: ${url}`, error);
      throw error;
    }
  };

  const fetchQuests = async (): Promise<Quest[]> => {
    return await sendRequestAsync('/quest', 'GET');
  };

  const completeQuest = async (id: string,): Promise<void> => {
    await sendRequestAsync(`/quest/${id}/complete`, 'PUT');
  };

  const addQuest = async (quest: Quest,): Promise<void> => {
    await sendRequestAsync('/quest', 'POST', quest);
  };

  const fetchRewards = async (): Promise<Reward[]> => {
    return await sendRequestAsync('/reward', 'GET');
  };

  const redeemReward = async (id: string,): Promise<void> => {
    await sendRequestAsync(`/reward/${id}/redeem`, 'PUT');
  };

  const addReward = async (reward: Reward,): Promise<void> => {
    await sendRequestAsync('/reward', 'POST', reward);
  };

  const deleteReward = async (id: string,): Promise<void> => {
    await sendRequestAsync(`/reward/${id}`, 'DELETE');
  };

  const fetchAccount = async (): Promise<Account> => {
    return await sendRequestAsync('/account', 'GET');
  };

  const fetchGroup = async (): Promise<Group> => {
    return await sendRequestAsync('/group', 'GET');
  };

  const createGroup = async (newGroup: NewGroup): Promise<void> => {
    await sendRequestAsync('/group', 'POST', newGroup);
  };

  const joinGroup = async (inviteCode: string): Promise<void> => {
    await sendRequestAsync(`/account/${inviteCode}`, 'PUT');
  };

  return {
    fetchQuests,
    completeQuest,
    addQuest,
    fetchRewards,
    redeemReward,
    addReward,
    deleteReward,
    fetchAccount,
    fetchGroup,
    createGroup,
    joinGroup,
  };
}

