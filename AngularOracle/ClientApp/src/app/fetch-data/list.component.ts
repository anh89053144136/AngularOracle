import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatSort, Sort } from '@angular/material/sort';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './list.component.html'
})
export class ListComponent {

  public forecasts: WeatherForecast[];
  public displayedColumns: string[] = ['id', 'temperatureC', 'summary', 'date'];
  
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {

    this.loadData('Id', 'ask');
  }

  loadData(orderField: string, orderDirection:string) {
    let url: string = this.baseUrl + 'weatherforecast/list?' +
      'orderField=' + orderField +
      '&orderDirection=' + orderDirection;

    this.http.get<WeatherForecast[]>(url).subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }

  sortData(sort: Sort) {
    this.loadData(sort.active, sort.direction);
  }
}

class WeatherForecast {
  id: number;
  date: string;
  temperatureC: number;
  summary: string;
}
