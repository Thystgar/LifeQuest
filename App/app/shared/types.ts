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
