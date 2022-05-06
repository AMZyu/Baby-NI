$(function() {

  $("#kendoVersion").text(kendo.version);
//************************************hourly data source************************************//
var gridDataSource = new kendo.data.DataSource({
  transport: {
    read: {
      url: "https://localhost:44361/api/Ui",
      dataType: "json"
    }
  },
  schema: {
    model: {
      fields: {
          DATETIME_KEY: { type: "date" },
          NETYPE: { type: "string" },
          NEALIAS: { type: "string" },
          LINK: { type: "string" },
          SLOT: { type: "number" },
          MAX_RX_LEVEL: { type: "number" },
          MAX_TX_LEVEL: { type: "number" },
          RSL_DEVIATION: { type: "number" }
      }
    }
  },
  pageSize: 10,
  sort: {
    field: "DATETIME_KEY",
    dir: "desc"
  }
});
//************************************hourly data source************************************//
//************************************Daily data source************************************//
var dataSource = new kendo.data.DataSource({
  transport: {
    read: {
      url: "https://localhost:44361/api/DailyUi",
      dataType: "json"
    }
  },
  schema: {
    model: {
      fields: {
          DATETIME_KEY: { type: "date" },
          NETYPE: { type: "string" },
          NEALIAS: { type: "string" },
          LINK: { type: "string" },
          SLOT: { type: "number" },
          MAX_RX_LEVEL: { type: "number" },
          MAX_TX_LEVEL: { type: "number" },
          RSL_DEVIATION: { type: "number" }
      }
    }
  },
  pageSize: 10,
  sort: {
    field: "DATETIME_KEY",
    dir: "desc"
  }
});
//************************************Daily data source************************************//
//***********************hourly chart***********************//
  var chart = $("#dataChart").kendoChart({
    dataSource: {
                              data: [],
                             sort: {
                             field: "DATETIME_KEY",
                               dir: "asc"
                               }
                             },
    title: {
      text: "kpi",
      font: "20px sans-serif",
      color: "#ff6800"
    },
    seriesDefaults: {
      type: "line"
    },
    series: [{
      field: "MAX_RX_LEVEL",
      categoryField: "DATETIME_KEY"
    }],
    seriesClick: function(e) {
      filterGrid(e.category);
    },
    axisLabelClick: function(e) {
      filterGrid(e.value);
    },
    categoryAxis: {
      labels: {
        rotation: -45,
        visual: function(e) {
          var visual = e.createVisual();
          visual.options.cursor = "default";
          return visual;
        }
      }
    },
    valueAxis: {
      title: {
        text: "RX"
      },
      labels: {
        format: "{0:n0}"
      }
    },
    tooltip: {
      visible: true,
      template: "#= category #: #= value # t"
    }
  }).data("kendoGrid");
  //***********************hourly chart***********************//
//*********************hourly grid*********************//
  var grid = $("#dataGrid").kendoGrid({
    dataSource: gridDataSource,
    dataBound: function(e) {
      var grid = e.sender,
          chart = $("#dataChart").data("kendoChart");

      chart.dataSource.data(grid.dataSource.data());
      grid.unbind("dataBound");
    },
    height: 400,
    pageable: true,
    sortable: true,
    filterable: true,
    columns: [{
                            field: "DATETIME_KEY",
                            title: "DATETIME_KEY",
                            format: "{0:MM/dd/yyyy hh:mm}"
                        }, {
                            field: "NETYPE",
                            title: "NETYPE"
                        },{
                            field: "NEALIAS",
                            title: "NEALIAS"
                        },
                        {
                            field: "LINK",
                            title: "LINK"
                        },{
                            field: "SLOT",
                            title: "MAX SLOT"
                        }, {
                            field: "MAX_RX_LEVEL",
                            title: "MAX_RX_LEVEL"
                        }, {
                            field: "MAX_TX_LEVEL",
                            title: "MAX_TX_LEVEL"
                        }, {
                            field: "RSL_DEVIATION",
                            title: "RSL_DEVIATION"
                        }
                    ]
  }).data("kendoGrid");
  //*********************hourly grid*********************//
  //*********************daily grid*********************//
  var grid = $("#dailyDataGrid").kendoGrid({
    dataSource: dataSource,
    // dataBound: function(e) {
    //   var grid = e.sender,
    //       chart = $("#dataChart").data("kendoChart");
    //
    //   chart.dataSource.data(grid.dataSource.data());
    //   grid.unbind("dataBound");
    // },
    height: 400,
    pageable: true,
    sortable: true,
    filterable: true,
    columns: [{
                            field: "DATETIME_KEY",
                            title: "DATETIME_KEY",
                            format: "{0:MM/dd/yyyy}"
                        }, {
                            field: "NETYPE",
                            title: "NETYPE"
                        },{
                            field: "NEALIAS",
                            title: "NEALIAS"
                        },
                        {
                            field: "LINK",
                            title: "LINK"
                        },{
                            field: "SLOT",
                            title: "MAX SLOT"
                        }, {
                            field: "MAX_RX_LEVEL",
                            title: "MAX_RX_LEVEL"
                        }, {
                            field: "MAX_TX_LEVEL",
                            title: "MAX_TX_LEVEL"
                        }, {
                            field: "RSL_DEVIATION",
                            title: "RSL_DEVIATION"
                        }
                    ]
  }).data("kendoGrid");
  //*********************daily grid*********************//
  $("#radiogroup").kendoRadioGroup({
                  items: ["RX", "TX", "RSL"],
                  layout: "vertical",
                  value: "RX",
                  change: function (e) {
                    if( e.newValue == "TX")
                    {    chart.options.series[0].field = "MAX_RX_LEVEL"
                         chart.refresh();}

                    else if (e.newValue == "RX"){
                      alert("sorry chart cannot be displayed");
                    }
                    else {
                      alert("sorry chart cannot be displayed");
                    }
                  }

              });

$("#buttongroup").kendoRadioGroup({
                              items: ["Daily", "Hourly"],
                              layout: "horizontal",
                              value: "Daily"
                          });
  });
