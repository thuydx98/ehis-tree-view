import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { TreeViewModel } from '../models/tree.model';

@Injectable({
  providedIn: 'root'
})
export class MenuService {

  private readonly apiUrl = 'http://localhost:5000/api/menues/';

  constructor(private http: HttpClient) { }

  public getMenues(): Observable<TreeViewModel[]> {
    return this.http.get(this.apiUrl)
      .pipe(
        map((res: TreeViewModel[]) => res)
      );
  }

  public saveMenu(id: number, displayName: string, parentId: number): Observable<TreeViewModel> {
    return this.http.post(this.apiUrl, { id, displayName, parentId })
      .pipe(
        map((res: TreeViewModel) => res)
      );
  }

  public deleteMenu(menuId: number, hasDeleteSubMenues = false) {
    return this.http.delete(this.apiUrl + menuId + '?hasDeleteSubMenues=' + hasDeleteSubMenues)
      .pipe(
        map((res: any) => res)
      );
  }
}
