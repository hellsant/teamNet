describe('Testing the graphic functions', function() {
  
  it('should add the value to the array', function() {
    var array = [];
    insertValues(array, 'test', 1);
    
    expect(array.length).toEqual(1);
  });
  
  it('should have the list of competences to draw', function() {
    var array = [];
    insertValues(array, 'test', 1);
    insertValues(array, 'test1', 2);
    insertValues(array, 'test2', 3);
    insertValues(array, 'test3', 4);
    insertValues(array, 'test4', 5);
    insertValues(array, 'test5', 6);
    
    expect(array.length).toEqual(6);
  });
  
  it('should show the name of the compotence', function() {
    var array = [];
    insertValues(array, 'test', 1);
    insertValues(array, 'test1', 2);
    insertValues(array, 'test2', 3);
    insertValues(array, 'test3', 4);
    insertValues(array, 'test4', 5);
    insertValues(array, 'test5', 6);
    
    expect(array[0].label).toEqual('test');
    expect(array[1].label).toEqual('test1');
    expect(array[2].label).toEqual('test2');
    expect(array[3].label).toEqual('test3');
  });
  
  it('should show the value of the compotence', function() {
    var array = [];
    insertValues(array, 'test', 1);
    insertValues(array, 'test1', 2);
    insertValues(array, 'test2', 3);
    insertValues(array, 'test3', 4);
    insertValues(array, 'test4', 5);
    insertValues(array, 'test5', 6);
    
    expect(array[0].data).toEqual(1);
    expect(array[1].data).toEqual(2);
    expect(array[2].data).toEqual(3);
    expect(array[3].data).toEqual(4);
  });
  
  it('should create the arrays for the graphic', function() {
    var array = [];
    createArrayOfArrays(array,3);
    expect(array.length).toEqual(3);
  });
  
  it('should create 3 arrays', function() {
    var array = [];
    var array1 = [];
    var array2 = [];
    
    createArrayOfArrays(array,3);
    createArrayOfArrays(array1,1);
    createArrayOfArrays(array2,5);
    
    expect(array.length).toEqual(3);
    expect(array1.length).toEqual(1);
    expect(array2.length).toEqual(5);
  });
  
  it('should have more than 3 objects', function() {
    var array = [];
    
    insertValues(array, 'test', 1);
    insertValues(array, 'test1', 2);
    insertValues(array, 'test2', 3);
    insertValues(array, 'test3', 4);
    insertValues(array, 'test4', 5);
    insertValues(array, 'test5', 6);
    
    expect(array.length).toEqual(6);
  });
  
  it('should insert value to especific position', function() {
    var array = [];
    
    createArrayOfArrays(array,5);
    insertValueToPosition(array, 1, 1, 5);
    
    var expected = [[1,5]];
    
    expect(array[1]).toEqual(expected);
    
    var expected = [[2,4]];
    insertValueToPosition(array, 2, 2, 4);
    
    expect(array[2]).toEqual(expected);
  });
  
  it('should have 5 objects into the array', function() {
    var array = [];
    
    createArrayOfArrays(array,5);
    
    insertValueToPosition(array, 0, 1, 5);
    insertValueToPosition(array, 1, 1, 5);
    insertValueToPosition(array, 2, 1, 5);
    insertValueToPosition(array, 3, 1, 5);
    insertValueToPosition(array, 4, 1, 5);
    
    var expected = 5;
    
    expect(array.length).toEqual(expected);
  });
  
  it('should be the same object', function() {
    var array = [];
    
    createArrayOfArrays(array,5);
    
    insertValueToPosition(array, 1, 1, 5);
    insertValueToPosition(array, 2, 1, 5);
    insertValueToPosition(array, 3, 1, 5);
    insertValueToPosition(array, 4, 1, 5);
    insertValueToPosition(array, 0, 1, 5);
    
    var expected = [[[1,5]],[[1,5]],[[1,5]],[[1,5]],[[1,5]]];
    
    expect(array).toEqual(expected);
  });
  
  it('should be the number 3 into the array', function() {
    var array = [];
    
    createArrayOfArrays(array,5);
    
    insertValueToPosition(array, 1, 2, 2);
    insertValueToPosition(array, 2, 1, 5);
    insertValueToPosition(array, 3, 1, 3);
    insertValueToPosition(array, 4, 1, 5);
    insertValueToPosition(array, 0, 1, 5);
    
    var expected = [ 1 , 3 ];
    var expected1 = [ 1 , 5 ];
    var expected2 = [ 2 , 2 ];
    
    expect(array[3][0]).toEqual(expected);
    expect(array[4][0]).toEqual(expected1);
    expect(array[1][0]).toEqual(expected2);
  });
  
  it('should have the label to the specific position', function() {
    var array = [];
    
    insertLabelToPosition(array, 1, 'testlabel');
    var expected = 'testlabel';
    expect(array[1]).toEqual(expected);
  });
  
  it('should have 10 labels into the array', function() {
    var array = [];
    
    insertLabelToPosition(array, 1, 'testlabel');
    insertLabelToPosition(array, 2, 'testlabel');
    insertLabelToPosition(array, 3, 'testlabel');
    insertLabelToPosition(array, 4, 'testlabel');
    insertLabelToPosition(array, 5, 'testlabel');
    insertLabelToPosition(array, 6, 'testlabel');
    insertLabelToPosition(array, 7, 'testlabel');
    insertLabelToPosition(array, 8, 'testlabel');
    insertLabelToPosition(array, 9, 'testlabel');
    insertLabelToPosition(array, 0, 'testlabel');
    
    expect(array.length).toEqual(10);
  });
  
  
});