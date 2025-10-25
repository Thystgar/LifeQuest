export class Account {
    name: string;
    points: number;
    groupId: string | null;
    termsAccepted: boolean;

    constructor(name: string = '', points: number = 0, groupId: string | null = null, termsAccepted: boolean = false) {
        this.name = name;
        this.points = points;
        this.groupId = groupId;
        this.termsAccepted = termsAccepted;
    }
}
