export class TreeViewModel {
    public id: number;
    public displayName;
    public parentId: number;
    public childrens: TreeViewModel[];
    public isOpen = false;

    constructor(id, displayName, parentId, childrens) {
        this.id = id;
        this.displayName = displayName;
        this.parentId = parentId;
        this.childrens = childrens;
        this.isOpen = false;
    }
}
