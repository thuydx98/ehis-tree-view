import { Component, OnInit } from '@angular/core';
import { MenuService } from '../../services/menu.service';
import { Message } from '../../commons/consts/message.const';
import { ToastrService } from 'ngx-toastr';
import { TreeViewModel } from '../../models/tree.model';

@Component({
  selector: 'app-tree-view',
  templateUrl: './tree-view.component.html',
  styleUrls: ['./tree-view.component.css']
})
export class TreeViewComponent implements OnInit {

  public loading = false;
  public list: TreeViewModel[];
  public selectedDeleteItem: TreeViewModel;

  constructor(
    private menuService: MenuService,
    private toasrtService: ToastrService) { }

  ngOnInit() {
    this.loading = true;
    this.menuService.getMenues().subscribe(
      (res) => {
        this.loading = false;
        this.list = res;
      }, (err) => {
        this.loading = false;
        this.toasrtService.error(Message.Menu.LOAD_MENU_LIST_FAIL);
      }
    );
  }

  openModal(e: Event, item: TreeViewModel) {
    this.selectedDeleteItem = item;

    if (item.childrens.length > 0) {
      const modal = document.getElementById('myModal');
      modal.style.display = 'block';
    } else {
      this.onDelete(e, true);
    }
    e.stopPropagation();
  }

  closeModal(e: Event) {
    const modal = document.getElementById('myModal');
    modal.style.display = 'none';
    this.selectedDeleteItem = null;
    e.stopPropagation();
  }

  public toggleNode(item: TreeViewModel) {
    item.isOpen = !item.isOpen;
  }

  public onAdd(e: Event, item: TreeViewModel): void {
    const inputName = prompt('Please enter new item');

    if (inputName !== null && inputName !== '') {
      this.saveItem(0, inputName, item ? item.id : null);

      if (item) {
        item.isOpen = true;
      }
    }

    e.stopPropagation();
  }

  public onEdit(e: Event, item: TreeViewModel): void {
    const inputName = prompt('Please enter new name', item.displayName);

    if (inputName !== null && inputName !== '') {
      this.saveItem(item.id, inputName, item.parentId);
    }

    e.stopPropagation();
  }

  public onDelete(e: Event, isDeleteAll: boolean, item: TreeViewModel = this.selectedDeleteItem) {
    if (item.childrens.length > 0 ||
      (item.childrens.length === 0 && confirm('Are you sure want to delele ' + item.displayName))) {
      if (this.selectedDeleteItem) {
        this.loading = true;
        this.menuService.deleteMenu(item.id, isDeleteAll).subscribe(
          (res) => {
            this.loading = false;
            this.toasrtService.success(Message.Menu.DELETE_MENU_SUCCESS);
            this.list = this.deleteItem(item.id, isDeleteAll);
          }, (err) => {
            this.loading = false;
            this.toasrtService.error(Message.Menu.DELETE_MENU_FAIL);
            return null;
          }
        );
      }
    }

    if (item.childrens.length > 0) {
      this.closeModal(e);
    }
  }

  private addNewItem(newItem: TreeViewModel, list = this.list): TreeViewModel[] {

    if (!newItem.parentId) {
      list.push(new TreeViewModel(newItem.id, newItem.displayName, newItem.parentId, []));
      return list;
    }

    list.forEach(item => {
      if (item.id === newItem.parentId) {
        item.childrens.push(new TreeViewModel(newItem.id, newItem.displayName, newItem.parentId, []));
      } else if (item.childrens) {
        item.childrens = this.addNewItem(newItem, item.childrens);
      }
    });

    return list;
  }

  private editItem(newItem: TreeViewModel, list = this.list): TreeViewModel[] {
    list.forEach(item => {
      if (item.id === newItem.id) {
        item.displayName = newItem.displayName;
      } else if (item.childrens) {
        item.childrens = this.editItem(newItem, item.childrens);
      }
    });

    return list;
  }

  private deleteItem(id, hasDeleteSubMenu, list = this.list): TreeViewModel[] {
    list.forEach((item, index) => {
      if (item.id === id) {
        if (!hasDeleteSubMenu) {
          list = list.concat(item.childrens);
        }
        list = list.filter(e => e.id !== id);
      } else if (item.childrens) {
        item.childrens = this.deleteItem(id, hasDeleteSubMenu, item.childrens);
      }
    });

    return list;
  }

  private saveItem(id: number, displayName: string, parentId: number): void {
    this.loading = true;
    const isNew = id === 0;
    this.menuService.saveMenu(id, displayName, parentId).subscribe(
      (res) => {
        this.loading = false;
        if (isNew) {
          this.list = this.addNewItem(res);
        } else {
          this.list = this.editItem(res);
        }
      }, (err) => {
        this.loading = false;
        this.toasrtService.error(Message.Menu.SAVE_MENU_FAIL);
      }
    );
  }
}
