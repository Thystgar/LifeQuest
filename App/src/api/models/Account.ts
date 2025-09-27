export class Account {
    name: string;
    points: number;
    groupId: string | null;

    constructor(name: string = '', points: number = 0, groupId: string | null = null) {
        this.name = name;
        this.points = points;
        this.groupId = groupId;
    }
}
