export class Account {
    name: string;
    points: number;

    constructor(name: string = '', points: number = 0) {
        this.name = name;
        this.points = points;
    }
}
