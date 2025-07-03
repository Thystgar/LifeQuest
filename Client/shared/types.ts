export interface Quest {
	id: string;
	name: string;
	description: string;
	value: number;
	status: QuestStatus; 
}

export enum QuestStatus {
    Accepted = "Accepted",
    Active = "Active",
    Completed = "Completed"
}

export interface Reward {
	id: string;
	name: string;
	description: string;
	value: number;
	redeemed: boolean;
}

export interface Account {
    name: string;
    points: number;
}

export function createReward(
  id: string = '',
  name: string = '',
  description: string = '',
  value: number = 0,
  redeemed: boolean = false
): Reward {
  return { id, name, description, value, redeemed };
}

export function createQuest(
  id: string = '',
  name: string = '',
  description: string = '',
  value: number = 0,
  status: QuestStatus = QuestStatus.Active
): Quest {
  return { id, name, description, value, status };
}
