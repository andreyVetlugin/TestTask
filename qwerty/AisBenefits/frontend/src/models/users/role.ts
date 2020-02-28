export default class Role {

    public id: string;
    public name: string;
    public permissions: string[];

    constructor(data: any) {
        this.id = data.id;
        this.name = data.name;
        this.permissions = data.permissions;
    }
}
