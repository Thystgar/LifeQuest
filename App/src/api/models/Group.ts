export class Group {
    id: string;
    name: string;
    description: string;
    inviteCode: string;

    constructor(name: string, description: string, inviteCode: string, id: string) {
        this.name = name;
        this.description = description;
        this.inviteCode = inviteCode;
        this.id = id;
    }
}

export class NewGroup {
    name: string;
    description: string;

    constructor(name: string, description: string) {
        this.name = name;
        this.description = description;
    }
}