
const boolOptinos = {
    yes: "Yes",
    no: "No"
}

const chooseOption = "Choose";

function getColumnsFromDataTable(selector, renders) {
    const columns = $(selector).find('thead > tr[data-dt-title] > th')
        //.filter((ix, th) => $(th).data('column-name'))
        .map((ix, th) => {
            const th$ = $(th);
            const name = th$.data('column-name') || null;
            const clmn = {
                name: name,
                data: name,
                orderable: th$.data('column-orderable')
            };

            if (renders) {
                clmn.render = renders["render" + name];
            }

            const dateFormat = th$.data('column-dateformat');
            if (dateFormat && !clmn.render) {
                clmn.render = $.fn.dataTable.render.rendermoment(dateFormat);
            }

            return clmn;
        });

    return columns.toArray();
}

function createSearchRow(table) {
    const isServerSide = table.settings()[0].oFeatures.bServerSide;

    table.columns()[0].forEach(columnIndex => {
        const column = table.column(columnIndex);
        if (column.visible()) {
            initializeFilter(column);
        }
    });

    table.on('column-visibility.dt', (e, settings, columnIndex, state) => {
        if (state) {
            const column = table.column(columnIndex);
            const header = $(column.header());
            if (!header.data('filter-initialized')) {
                initializeFilter(column);
            }
        }
    });

    $('.table-filter').click(e => e.stopPropagation());

    function initializeFilter(column) {
        const dataIndex = column.index();
        const displayIndex = column.index('visible');
        const header = $(column.header());
        const searchCell = $(header.closest('table').find('tr[data-dt-filter] > th')[displayIndex]);

        const classAtr = searchCell.attr('class');
        if (!classAtr)
            return;

        const filterClass = classAtr.split(/\s+/).find(c => c.substring(0, 7) === 'filter-');
        if (!filterClass) {
            return;
        }

        searchCell.addClass('filter-header');

        const filterType = filterClass.substring(7);
        const isDateRange = filterType === "daterange";

        switch (filterType) {
            case 'text':
                searchCell.append('<input type="text" class="table-filter" data-filter/><span class="fas fa-search errspan"></span>');
                setStoragedFilter(table, column, searchCell);
                break;

            case 'date':
                searchCell.append('<input type="date" class="table-filter" data-filter/><span class="fas fa-search errspan"></span>');
                setStoragedFilter(table, column, searchCell);
                break;

            case 'daterange':
                searchCell.append(`<div>
                                        <input type= "date" class="table-filter" data-filter/>
                                        <span>עד</span>
                                        <input type= "date" class="table-filter" data-filter/>
                                    </div>`);

                setStoragedFilter(table, column, searchCell, (inputs, val) => {
                    var splited = val.split(',');
                    $(inputs[0]).val(splited[0]);
                    $(inputs[1]).val(splited[1]);
                });

                if (!isServerSide) {
                    const inputs = searchCell.find('input');

                    $.fn.dataTable.ext.search.push((settings, data) => {
                        const min = toDate(inputs[0].value);
                        const max = toDate(inputs[1].value);
                        const date = toDate(data[dataIndex]);
                        return min === null && (max === null || date <= max) ||
                            min <= date && (max === null || date <= max);
                    });
                }
                break;

            case 'bool':
                searchCell.append(`<select data-filter>
                                        <option value=''>${chooseOption}</option>
                                        <option value="true">${boolOptinos.yes}</option>
                                        <option value="false">${boolOptinos.no}</option>
                                    </select>`);
                setStoragedFilter(table, column, searchCell);
                break;

            case 'select':
                populateSelect(table, column, searchCell);
                break;
        }

        searchCell.on('keyup change', '[data-filter]', e => {
            const target = e.currentTarget;
            const $target = $(target);

            let searchKey = '';

            if (searchCell.data('bytext')) {
                searchKey = target.options[target.selectedIndex].text;
            }
            else {
                if (isDateRange) {
                    const inputs = searchCell.find('input');
                    searchKey = $(inputs[0]).val() + ',' + $(inputs[1]).val();
                }
                else {
                    const val = $target.val();
                    if (val && val !== 'multiselect-all') {
                        searchKey = val.toString();
                    }
                }
            }

            if (!isServerSide && isDateRange) {
                table.draw();
            }
            else {
                table.settings()[0].jqXHR.abort();
                column.search(searchKey).draw();
            }

            if (searchKey && searchKey.length > 0) {
                searchCell.addClass("in-search");
            } else {
                searchCell.removeClass("in-search");
            }
        });

        header.attr('data-filter-initialized', true);
    }

    function populateSelect(table, column, searchCell) {
        const filterList = searchCell.data('filter-list');
        if (filterList === "Month") {
            const options = [];
            for (let i = 0; i < 12; i++) {
                const month = new Date(2000, i, 1).toLocaleString('he-il', { month: "long" });
                options.push({ Value: i + 1, Text: month });
            }

            populateHeaderSelectFromArray(table, column, searchCell, options);
        }
        else {
            const appendToHeader = () => populateSelectFromUrl(table, column, searchCell);
            appendToHeader();

            const dependOn = searchCell.data('filter-source-dependon');
            if (dependOn) {
                $(dependOn).change(appendToHeader);
            }
        }
    }

    async function populateSelectFromUrl(table, column, searchCell) {
        const source = searchCell.data('filter-source') || searchCell.data('ext');

        if (!source) {
            throw `url of columns ${column.index()} is undefined.`;
        }

        let url =  source;

        const dependOn = searchCell.data('filter-source-dependon');
        const dependOnParameterName = searchCell.data('filter-source-parametername');

        if (dependOn && dependOnParameterName) {
            let dependOnVal = $(dependOn).val();

            if (!dependOnVal) {
                dependOnVal = $(dependOn).find('select').val();
            }

            if (dependOnVal) {
                url += '?';
                url += typeof dependOnVal === "object" ?
                    dependOnVal.map(v => `${dependOnParameterName}=${v}`).join('&') :
                    `${dependOnParameterName}=${dependOnVal}`;
            }
        }

        const options = await $.get(url);
        populateHeaderSelectFromArray(table, column, searchCell, options);
    }

    function populateHeaderSelectFromArray(table, column, searchCell, options) {
        searchCell.empty().append('<select multiple data-filter hidden><option></option></select>');
        const select = searchCell.find("select");
        options.forEach((item, index) => {
            node = document.createElement("option"); // Create a <Option> node
            $(node).val(item.Value || item.Id);
            textnode = document.createTextNode(item.Text || item.Name); // Create a text node
            node.appendChild(textnode); // Append the text to <Option>
            select.append(node);
        });

        select.multiselect({
            enableFiltering: true,
            includeSelectAllOption: true
        });

        //colorSingleDropdown();

        const searchString = getStoragedFilter(table, column);
        if (searchString) {
            select.val(searchString.split(",")).multiselect("refresh");
            searchCell.addClass("in-search");
        }
    }

    function getStoragedFilter(table, column) {
        const index = column.index();
        return table.settings()[0].aoPreSearchCols[index].sSearch;
    }

    function setStoragedFilter(table, column, searchCell, valueProccess) {
        const $input = searchCell.find('input, select');
        if (column.visible()) {
            const val = getStoragedFilter(table, column);
            if (val) {
                if (valueProccess) {
                    valueProccess($input, val);
                }
                else {
                    $input.val(val);
                }
                searchCell.addClass("in-search");
            }
        }
    }

    function toDate(str) {
        if (str === "" || str === null || str === 'undefind') {
            return null;
        }

        const parts = str.split("/");
        return new Date(Number(parts[2]), Number(parts[1]) - 1, Number(parts[0]));
    }
}

rendermoment = $.fn.dataTable.render.rendermoment = to => {
    return (data, type) => {
        if (!data) {
            return data;
        }

        const m = moment(data, moment.ISO_8601, true);

        // Order and type get a number value from Moment, 
        // everything else sees the rendered value
        return m.format(type === 'sort' || type === 'type' ? 'x' : to);
    };
};
