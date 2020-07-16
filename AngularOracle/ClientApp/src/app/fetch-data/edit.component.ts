import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './edit.component.html'
})
export class EditComponent {

  private model: WeatherForecast;

  constructor(
    private activateRoute: ActivatedRoute,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
    
    this.model = new WeatherForecast();
    //this.model.date = "1999-01-02";
    this.model.temperatureC = 20;
    this.model.summary = "10";

    var id = 0;
    activateRoute.params.subscribe(params => id = params['id']);
    
    this.loadData(id);
  }

  loadData(id: number) {
    let url: string = this.baseUrl + 'weatherforecast/' + id;

    this.http.get<WeatherForecast>(url).subscribe(result => {
      this.model = result;
    }, error => console.error(error));
  }

  public OnSaveClick() {
    this.http.post(this.baseUrl + 'weatherforecast', this.model).subscribe(result => {
      debugger;
    }, error => console.error(error));
  }
}

class WeatherForecast {
  date: string;
  temperatureC: number;
  summary: string;
}
