export enum QuestStatus {
    Accepted = "Accepted",
    Active = "Active",
    Completed = "Completed"
}

export class Quest {
    id: string;
    name: string;
    description: string;
    value: number;
    status: QuestStatus;

    constructor(
        id: string = '',
        name: string = '',
        description: string = '',
        value: number = 0,
        status: QuestStatus = QuestStatus.Active
    ) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.value = value;
        this.status = status;
    }
}
