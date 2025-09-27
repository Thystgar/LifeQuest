export class Reward {
    id: string;
    name: string;
    description: string;
    value: number;
    redeemed: boolean;
    groupId: string;

    constructor(
        id: string = '',
        name: string = '',
        description: string = '',
        value: number = 0,
        redeemed: boolean = false,
        groupId: string = ''
    ) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.value = value;
        this.redeemed = redeemed;
        this.groupId = groupId;
    }
}
